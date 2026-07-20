import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ImmsbcAccountAccessChangePage } from './immsbc-account-access-change.page';

describe('ImmsbcAccountAccessChangePage', () => {
  let component: ImmsbcAccountAccessChangePage;
  let fixture: ComponentFixture<ImmsbcAccountAccessChangePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImmsbcAccountAccessChangePage],
    }).compileComponents();

    fixture = TestBed.createComponent(ImmsbcAccountAccessChangePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
