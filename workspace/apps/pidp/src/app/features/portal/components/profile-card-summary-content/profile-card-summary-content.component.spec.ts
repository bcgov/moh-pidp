import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileCardSummaryContentComponent } from './profile-card-summary-content.component';

describe('ProfileCardSummaryContentComponent', () => {
  let component: ProfileCardSummaryContentComponent;
  let fixture: ComponentFixture<ProfileCardSummaryContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfileCardSummaryContentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileCardSummaryContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
