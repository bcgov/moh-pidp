import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollegeLicenceInformationComponent } from './college-licence-information.component';

describe('CollegeLicenceInformationComponent', () => {
  let component: CollegeLicenceInformationComponent;
  let fixture: ComponentFixture<CollegeLicenceInformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CollegeLicenceInformationComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CollegeLicenceInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
