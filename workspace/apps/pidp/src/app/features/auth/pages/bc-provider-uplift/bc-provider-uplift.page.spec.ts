import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BcProviderUpliftPage } from './bc-provider-uplift.page';

describe('BcProviderUpliftPage', () => {
  let component: BcProviderUpliftPage;
  let fixture: ComponentFixture<BcProviderUpliftPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BcProviderUpliftPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BcProviderUpliftPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
