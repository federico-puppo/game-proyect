# Sistema UI de Selección de Ítems – Comunicación con InventoryManager

## 🧩 Objetivo
Este sistema permite al jugador **ver, arrastrar y seleccionar ítems recogidos** durante el gameplay, para decidir cuáles llevar a la próxima escena.

La escena `Mental_UI_Scene` actúa como una pantalla mental intermedia donde el jugador elige su estrategia.

---

## 🔄 Comunicación con InventoryManager

La UI **no tiene un inventario propio**, sino que se comunica directamente con:

- `InventoryManager.Instance.GetItems()` → para mostrar todos los ítems disponibles (ya recogidos).
- `InventoryManager.Instance.EquipItem(...)` → para guardar la decisión del jugador al hacer clic en el botón de avanzar de escena.

---

## 🗃 Estructura Clave

| Elemento                  | Ubicación                | Rol                                                                 |
|---------------------------|---------------------------|----------------------------------------------------------------------|
| `GameItemData`            | `Assets/Items/` (MAIN)    | ScriptableObjects que contienen los datos de cada ítem.             |
| `GameItemHolder.cs`       | En cada prefab de ítem    | Asocia un prefab 3D de UI a su `GameItemData`.                      |
| `PrefabDictionary.asset`  | `Assets/UI/`              | Diccionario que relaciona `GameItemData` → `Prefab`.                |
| `AvailableItemsFromInventory.cs` | En escena UI       | Muestra los ítems equipados en la UI usando el diccionario.         |
| `UIManager.cs`            | En escena UI              | Maneja slots, botón de siguiente escena y guarda ítems elegidos.    |

---

## 🧰 Cómo agregar un nuevo ítem (paso a paso)

1. **Crear ScriptableObject**
   - Ir a `Assets/Items/`
   - Click derecho → `Create > Game > Item`
   - Llenar campos: `itemName`, `type`, `description`, etc.

2. **Crear un nuevo prefab 3D para UI**
   - Crear cubo 3D con imagen, outline y scripts (`DragItem`, `GameItemHolder`)
   - En el `GameItemHolder`, asignar el `GameItemData` recién creado.

3. **Registrar en el PrefabDictionary**
   - Abrir `PrefabDictionary.asset`
   - Agregar nueva entrada con:
     - `itemData`: el nuevo `GameItemData`
     - `prefab`: el nuevo prefab de UI

4. ¡Listo! Si el jugador recoge ese ítem en el gameplay, aparecerá en la UI automáticamente.

---

## 📌 Consideraciones

- La **asociación entre prefabs y lógica del inventario** se da exclusivamente por el campo `itemData` del componente `GameItemHolder`.
- **No importa el nombre del prefab**, siempre que su `itemData` sea el correcto y esté registrado en el `PrefabDictionary`.
- El sistema **no usa strings sueltos**: todo está vinculado a objetos reales (`GameItemData`, `GameItem`).

---

## ✅ Loop funcional

```text
Scene de acción → Jugador recoge ítems → InventoryManager los guarda
        ↓
Scene Mental_UI_Scene → UI los muestra con PrefabDictionary
        ↓
Jugador arrastra 4 ítems → UIManager guarda decisión en InventoryManager
        ↓
Scene siguiente → GameManager detecta estilo basado en ítems
```

---

Este sistema fue diseñado para **respetar la estructura del proyecto MAIN** sin modificarla, y puede escalar fácilmente a decenas de ítems.
