import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalAccountsPage } from './external-accounts.page';

describe('ExternalAccountsPage', () => {
  let component: ExternalAccountsPage;
  let fixture: ComponentFixture<ExternalAccountsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExternalAccountsPage],
    }).compileComponents();

    fixture = TestBed.createComponent(ExternalAccountsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
