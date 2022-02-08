import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollegeLicenceInformationPage } from './college-licence-information.page';

describe('CollegeLicenceInformationPage', () => {
  let component: CollegeLicenceInformationPage;
  let fixture: ComponentFixture<CollegeLicenceInformationPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CollegeLicenceInformationPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CollegeLicenceInformationPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
