import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ImmsbcPasswordResetPage } from './immsbc-password-reset.page';

describe('ImmsbcPasswordResetPage', () => {
  let component: ImmsbcPasswordResetPage;
  let fixture: ComponentFixture<ImmsbcPasswordResetPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImmsbcPasswordResetPage],
    }).compileComponents();

    fixture = TestBed.createComponent(ImmsbcPasswordResetPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
