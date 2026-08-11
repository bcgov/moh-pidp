import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ImmsbcManagePharmacyPage } from './immsbc-manage-pharmacy.page';

describe('ImmsbcManagePharmacyPage', () => {
  let component: ImmsbcManagePharmacyPage;
  let fixture: ComponentFixture<ImmsbcManagePharmacyPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImmsbcManagePharmacyPage],
    }).compileComponents();

    fixture = TestBed.createComponent(ImmsbcManagePharmacyPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});