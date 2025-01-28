import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalGuestInvitationComponent } from './external-guest-invitation.page';

describe('ExternalGuestInvitationComponent', () => {
  let component: ExternalGuestInvitationComponent;
  let fixture: ComponentFixture<ExternalGuestInvitationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExternalGuestInvitationComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ExternalGuestInvitationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
