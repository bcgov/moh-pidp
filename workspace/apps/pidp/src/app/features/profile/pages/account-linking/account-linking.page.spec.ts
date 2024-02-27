import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountLinkingPage } from './account-linking.page';

describe('AccountLinkingPage', () => {
  let component: AccountLinkingPage;
  let fixture: ComponentFixture<AccountLinkingPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccountLinkingPage],
    }).compileComponents();

    fixture = TestBed.createComponent(AccountLinkingPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
