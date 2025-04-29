import { InstructionCardComponent } from './instruction-card.component';

describe('InstructionCardComponent', () => {
  let component: InstructionCardComponent;

  beforeEach(() => {
    component = new InstructionCardComponent();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have default @Input() values', () => {
    expect(component.isActive).toBe(false);
    expect(component.isCompleted).toBe(false);
    expect(component.cardData).toBeUndefined();
  });

  it('should accept @Input() values', () => {
    component.cardData = { foo: 'bar' };
    component.isActive = true;
    component.isCompleted = true;
    expect(component.cardData).toEqual({ foo: 'bar' });
    expect(component.isActive).toBe(true);
    expect(component.isCompleted).toBe(true);
  });

  it('should initialize email as empty string', () => {
    expect(component.email).toBe('');
  });

  it('should emit continueEvent with value when onContinue is called', () => {
    const emitSpy = jest.spyOn(component.continueEvent, 'emit');
    const testValue = { test: 123 };
    component.onContinue(testValue);
    expect(emitSpy).toHaveBeenCalledWith(testValue);
  });

  it('should emit continueEvent with undefined when onContinue is called without value', () => {
    const emitSpy = jest.spyOn(component.continueEvent, 'emit');
    component.onContinue();
    expect(emitSpy).toHaveBeenCalledWith(undefined);
  });
});
