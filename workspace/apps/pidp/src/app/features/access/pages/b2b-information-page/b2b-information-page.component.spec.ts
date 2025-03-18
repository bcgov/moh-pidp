import { ComponentFixture, TestBed } from '@angular/core/testing';

import { B2bInformationPageComponent } from './b2b-information-page.component';

describe('B2bInformationPageComponent', () => {
  let component: B2bInformationPageComponent;
  let fixture: ComponentFixture<B2bInformationPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [B2bInformationPageComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(B2bInformationPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
