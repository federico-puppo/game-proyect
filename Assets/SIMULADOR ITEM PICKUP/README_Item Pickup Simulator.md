# Prefab de Testeo - Simulación de Ítems Recogidos

Este prefab simula la recolección de ítems en una escena de acción para facilitar pruebas del sistema de UI sin necesidad de recorrer el juego.

### 🔧 ¿Cómo funciona?

- En `Start()`, toma la lista de prefabs (`simulatedItemPrefabs[]`).
- Extrae su `GameItemData` desde el componente `GameItemHolder`.
- Agrega esos ítems al `InventoryManager` como si el jugador los hubiera recogido.
- No los equipa: quedan como ítems disponibles para que el jugador los seleccione en la escena de UI mental.

### 🧩 Uso

1. Arrastrá el prefab de testeo a una escena como `Ofice` o `SampleScene`.
2. Asigná el botón que lleva a la escena `Mental_UI_Scene` (campo `goToUISceneButton`).
3. Asigná los ítems a simular (campo `simulatedItemPrefabs[]`).

### ⚠ Recomendación

Usá este prefab solo durante desarrollo. Eliminálo al integrar la lógica real de recolección en gameplay.
