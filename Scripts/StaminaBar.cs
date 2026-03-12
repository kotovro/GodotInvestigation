using Godot;

public partial class StaminaBar : TextureProgressBar
{
	private StaminaComponent _staminaComponent;
	private float _currentStamina;

	public void Initialize(StaminaComponent staminaComponent)
	{
		_staminaComponent = staminaComponent;
		_staminaComponent.StaminaChanged += OnStaminaChanged;

		MaxValue = _staminaComponent.MaxStamina;
		_currentStamina = _staminaComponent.CurrentStamina;
		Value = _currentStamina;
	}

	public void OnStaminaChanged(float delta, float maximumStamina)
	{
		_currentStamina = delta; // if delta is current stamina
		Value = _currentStamina;
	}

	public override void _Process(double delta)
	{
		Value = Mathf.Lerp(Value, _currentStamina, 5f * (float)delta);
	}
}
