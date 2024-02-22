import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { PreferredNameFormComponent } from './preferred-name-form.component';

describe('PreferredNameFormComponent', () => {
  let component: PreferredNameFormComponent;
  let fixture: ComponentFixture<PreferredNameFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PreferredNameFormComponent);
    component = fixture.componentInstance;
    component.form = new FormGroup({
      preferredFirstName: new FormControl(''),
      preferredMiddleName: new FormControl(''),
      preferredLastName: new FormControl(''),
    });
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
