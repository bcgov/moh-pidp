import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatCardModule } from '@angular/material/card';

import { CardComponent } from '../card/card.component';
import { CardSummaryComponent } from './card-summary.component';

describe('CardSummaryComponent', () => {
  let component: CardSummaryComponent;
  let fixture: ComponentFixture<CardSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CardSummaryComponent, CardComponent],
      imports: [MatCardModule],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CardSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
