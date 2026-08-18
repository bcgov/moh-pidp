import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideEnvironmentNgxMask } from 'ngx-mask';
import { ImmsbcCreatePharmacyPage } from './immsbc-create-pharmacy.page';

describe('ImmsbcCreatePharmacyPage', () => {
  let component: ImmsbcCreatePharmacyPage;
  let fixture: ComponentFixture<ImmsbcCreatePharmacyPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImmsbcCreatePharmacyPage, NoopAnimationsModule],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([]),
        provideEnvironmentNgxMask()
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ImmsbcCreatePharmacyPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});