using Game.Field.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Field.Input.Scripts
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        
        private FieldController _fieldController;
        private bool _clickRequested;
        
        [SerializeField] private InputActionAsset inputActionAsset;
        private InputAction _clickInputAction;
        
        private RaycastHit2D[] _raycastHitsByClick = new RaycastHit2D[10];

        private Tile _chosenTile;

        [Inject]
        private void Construct(FieldController fieldController)
        {
            _fieldController = fieldController;
        }

        private void Awake()
        {
            _clickInputAction = inputActionAsset.FindActionMap("Player").FindAction("Click");
            _clickInputAction.Enable();
        }

        private void Update()
        {
            ClickInput();
        }

        private void FixedUpdate()
        {
            ClickBehavior();
        }

        public void RegenerateField() => _fieldController.RegenerateTiles();

        private void ClickInput()
        {
            if (_clickInputAction.WasPressedThisFrame())
                _clickRequested = true;
        }

        private void ClickBehavior()
        {
            if (_clickRequested)
            {
                _clickRequested = false;
                
                var ray = camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                var size = Physics2D.GetRayIntersectionNonAlloc(ray, _raycastHitsByClick);

                Tile clickedTile = null;
                    
                for (int i = 0; i < size; i++)
                {
                    if (_raycastHitsByClick[i].collider.gameObject.TryGetComponent(out Tile currTile))
                    {
                        if (clickedTile == null)
                            clickedTile = currTile;
                        
                        if (currTile.Z > clickedTile.Z)
                            clickedTile = currTile;
                    }
                }
                
                if (clickedTile != null)
                {
                    if (!clickedTile.IsAvailable)
                    {
                        _chosenTile = null;
                        return;
                    }

                    if (_chosenTile == null)
                    {
                        _chosenTile = clickedTile;
                        return;
                    }

                    if (clickedTile == _chosenTile)
                    {
                        _chosenTile = null;
                        return;
                    }

                    if (_chosenTile.Name == clickedTile.Name)
                    {
                        Destroy(clickedTile);
                        Destroy(_chosenTile);
                        _chosenTile = null;
                    }
                    else
                    {
                        _chosenTile = null;
                    }
                }
            }
        }
    }
}
