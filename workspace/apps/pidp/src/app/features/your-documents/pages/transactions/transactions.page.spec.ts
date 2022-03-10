import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionsPage } from './transactions.page';

describe('TransactionsPage', () => {
  let component: TransactionsPage;
  let fixture: ComponentFixture<TransactionsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TransactionsPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TransactionsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
