import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ImmsbcAddingAccountsPage } from './immsbc-adding-accounts.page';

describe('ImmsbcAddingAccountsPage', () => {
  let component: ImmsbcAddingAccountsPage;
  let fixture: ComponentFixture<ImmsbcAddingAccountsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImmsbcAddingAccountsPage],
    }).compileComponents();

    fixture = TestBed.createComponent(ImmsbcAddingAccountsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
