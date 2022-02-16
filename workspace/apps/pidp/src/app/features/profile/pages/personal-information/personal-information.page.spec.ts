import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalInformationPage } from './personal-information.page';

describe('PersonalInformationPage', () => {
  let component: PersonalInformationPage;
  let fixture: ComponentFixture<PersonalInformationPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PersonalInformationPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonalInformationPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
