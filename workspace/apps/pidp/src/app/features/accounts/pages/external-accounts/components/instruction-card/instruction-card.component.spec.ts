import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InstructionCardComponent } from './instruction-card.component';
import { InstructionCard } from './instruction-card.model';

describe('InstructionCardComponent', () => {
  let component: InstructionCardComponent;
  let fixture: ComponentFixture<InstructionCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InstructionCardComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(InstructionCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have default input values', () => {
    expect(component.cardData).toEqual({} as InstructionCard);
    expect(component.routePath).toBe('');
    expect(component.isActive).toBe(false);
    expect(component.isCompleted).toBe(false);
  });

  it('should accept input values', () => {
    const cardData: InstructionCard = {
      title: 'Test',
      description: 'Desc',
    } as InstructionCard;
    component.cardData = cardData;
    component.routePath = '/test';
    component.isActive = true;
    component.isCompleted = true;
    fixture.detectChanges();

    expect(component.cardData).toBe(cardData);
    expect(component.routePath).toBe('/test');
    expect(component.isActive).toBe(true);
    expect(component.isCompleted).toBe(true);
  });
});
