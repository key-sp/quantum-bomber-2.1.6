using System;
using Photon.Deterministic;

namespace Quantum
{
  public unsafe class Grid
  {
    private GridSettings _settings = default;
    private Cell[] _cells = null; 
    private Cell* _cellsPtr = default;
    
    public void CopyFromUser(Grid otherGrid)
    {
      _settings = otherGrid._settings;
      Array.Copy(otherGrid._cells, _cells, otherGrid._cells.Length);
    }

    public void SerializeUser(FrameSerializer serializer)
    {
      // --- Settings Serialization
      // Technically unnecessary as the settings do not change ever after the initial creation of the frame
      // This is kept for educational & completion's sake.
      GridSettings gridSettings = _settings;
      GridSettings.Serialize(&gridSettings, serializer);
      _settings = gridSettings;
     
      // --- Grid Serialization
      // Since the array representing the grid is of fixed length, the length does not need to be serialized separately.
      for (var i = 0; i < _cells.Length; i++) {
        var current = _cells[i];

        // Converts the types inside the struct to serializable values
        Int32 type = (Int32) current.Type;
        
        // Serializes every part of the struct
        serializer.Stream.Serialize(ref type);
        
        // Reassigns the values upon deserialization
        _cells[i].Type = (CellType) type;
      }
    }

    public void DumpFrameUser(ref string dump)
    {
      // Prints the entire grid's cell values in case of a desync. 
      var printer = new FramePrinter();

      printer.AddLine("VisibilityManager:");
      printer.ScopeBegin();
      foreach (var cell in _cells)
      {
        printer.AddLine($"Cell Type = {cell.Type}");
      }
      printer.ScopeEnd();
      dump += printer.ToString();
    }

    public void Init(Frame f)
    {
      // Enforces and adjust grid settings. This is done in-simulation rather than in Unity to make it cheat proof.
      InitGridSettings(f);
      
      // Allocate Array for the Grid
      _cells = new Cell[_settings.GetGridSize()];

      // Save the Pointer in order to first element in the array to be able to do pointer math when accessing it. Otherwise, the Cell (struct) would have to be operated on by copy & set which would create significant overhead given the amount of times the grid is being accessed by the systems.
      fixed (Cell* cellsPtr = &_cells[0]) {
        _cellsPtr = cellsPtr;
      }

      // Initialize the grid values
      InitGridCells(f);
    }

    public void Clear(Frame f)
    {
      _cells = default;
    }

    private void InitGridSettings(Frame f)
    {
      _settings.Width = AdjustValue(f.RuntimeConfig.GridSize);
      _settings.Height = AdjustValue(f.RuntimeConfig.GridSize);
      _settings.Type = GridType.Square;

      // Set as required
      _settings.CellSize = f.RuntimeConfig.CellSize;
      _settings.SpawnPointMinDistance = f.RuntimeConfig.MinDistanceBetweenSpawnPoints;
      _settings.AutoAdjustSize = f.RuntimeConfig.AutoAdjustToPlayerCount;
    }
    
    private void InitGridCells(Frame f)
    {
      var width = _settings.GetWidth();
      var height = _settings.GetHeight();

      var cellEmpty = default(Cell);
      cellEmpty.Type = CellType.Empty;

      var cellBlocked = default(Cell);
      cellBlocked.Type = CellType.BlockFix;

      var cellDestroyable = default(Cell);
      cellDestroyable.Type = CellType.BlockDestroyable;

      // Set up outer cells (boundaries)
      for (var x = 0; x < width; x++)
      {
        SetCell(f, cellBlocked, x, 0);
        SetCell(f, cellBlocked, x, height - 1);
      }

      for (var y = 0; y < height; y++)
      {
        SetCell(f, cellBlocked, 0, y);
        SetCell(f, cellBlocked, width - 1, y);
      }

      // Set up inside cells
      for (var x = 1; x < width - 1; x++) {
        for (var y = 1; y < height - 1; y++) {
          if (x % 2 == 0 && y % 2 == 0) {
            SetCell(f, cellBlocked, x, y);
          }
          else {
            SetCell(f, cellEmpty, x, y);
          }
        }
      }

      // Set up destroyable cells
      var chance = f.RuntimeConfig.AmountOfDestroyableCells;
      var rng = default(RNGSession);

      for (var x = 1; x < width - 1; x++) {
        for (var y = 1; y < height - 1; y++) {
          // Skip fixed cells
          if (x % 2 == 0 && y % 2 == 0) continue;

          // Run probability for this to be a destroyable cell
          if (rng.NextInclusive(0, 100) > chance) continue;

          SetCell(f, cellDestroyable, x, y);
        }
      }
    }
    
    private byte AdjustValue(byte value)
    {
      // Required to ensure an odd amount of tiles in either direction.
      // A pair number would result in a missing row or column.
      return value % 2 == 0 ? ++value : value;
    }
    
    public byte GetGridHeight()
    {
      return _settings.GetHeight();
    }

    public byte GetGridWidth()
    {
      return _settings.GetWidth();
    }

    private int GetGridSize()
    {
      return _settings.GetGridSize();
    }
    
    public Cell* GetCellPtr(FPVector2 position)
    {
      return GetCellPtr(FPMath.RoundToInt(position.X), FPMath.RoundToInt(position.Y));
    }

    public Cell* GetCellPtr(int x, int y)
    {
      var index = GetIndex(x, y);
      return (_cellsPtr + index);
    }

    private void SetCell(Frame f, Cell cellData, int x, int y)
    {
      var index = GetIndex(x, y);
      (_cellsPtr + index)->Type = cellData.Type;
    }

    private int GetIndex(int x, int y)
    {
      var index = y * GetGridWidth() + x;
      // Defensive version. Dropped to avert silent errors.
      // index = index > _grid.GetLength(0) ? -1 : index; 
      return index;
    }
  }
}