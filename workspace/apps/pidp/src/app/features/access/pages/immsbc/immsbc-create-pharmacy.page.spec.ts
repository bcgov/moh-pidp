import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ImmsbcCreatePharmacyPage } from './immsbc-create-pharmacy.page';

describe('ImmsbcCreatePharmacyPage', () => {
  let component: ImmsbcCreatePharmacyPage;
  let fixture: ComponentFixture<ImmsbcCreatePharmacyPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImmsbcCreatePharmacyPage],
    }).compileComponents();

    fixture = TestBed.createComponent(ImmsbcCreatePharmacyPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});