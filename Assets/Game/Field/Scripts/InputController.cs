using UnityEngine;
using Zenject;

namespace Game.Field.Scripts
{
    public class InputController : MonoBehaviour
    {
        private FieldController _fieldController;

        [Inject]
        private void Construct(FieldController fieldController)
        {
            _fieldController = fieldController;
        }

        public void RegenerateField() => _fieldController.RegenerateTiles();
    }
}
