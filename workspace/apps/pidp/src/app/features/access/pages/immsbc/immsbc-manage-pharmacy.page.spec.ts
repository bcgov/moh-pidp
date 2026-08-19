import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ImmsbcManagePharmacyPage } from './immsbc-manage-pharmacy.page';

describe('ImmsbcManagePharmacyPage', () => {
  let component: ImmsbcManagePharmacyPage;
  let fixture: ComponentFixture<ImmsbcManagePharmacyPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImmsbcManagePharmacyPage, NoopAnimationsModule],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([])
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ImmsbcManagePharmacyPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});