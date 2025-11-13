import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitedAccountCardComponent } from './invited-account-card.component';

describe('InvitedAccountCardComponent', () => {
  let component: InvitedAccountCardComponent;
  let fixture: ComponentFixture<InvitedAccountCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvitedAccountCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvitedAccountCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
