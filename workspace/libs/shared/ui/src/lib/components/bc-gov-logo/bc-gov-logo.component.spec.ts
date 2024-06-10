import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BcGovLogoComponent } from './bc-gov-logo.component';

describe('BcGovLogoComponent', () => {
  let component: BcGovLogoComponent;
  let fixture: ComponentFixture<BcGovLogoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({}).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BcGovLogoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
