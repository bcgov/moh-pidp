import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingEndorsementComponent } from './pending-endorsement.component';

describe('PendingEndorsementComponent', () => {
  let component: PendingEndorsementComponent;
  let fixture: ComponentFixture<PendingEndorsementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [],
    }).compileComponents();

    fixture = TestBed.createComponent(PendingEndorsementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
