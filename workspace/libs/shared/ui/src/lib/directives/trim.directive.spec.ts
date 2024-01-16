import { Component, DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

import { TrimDirective } from './trim.directive';

@Component({
  selector: 'ui-input-test',
  template: `<input uiTrim type="text" />
    <div [formGroup]="form">
      <input uiTrim formControlName="name" />
    </div>`,
  standalone: true,
  imports: [TrimDirective, ReactiveFormsModule],
})
class UnitTestComponent {
  public form: FormGroup;

  public constructor() {
    this.form = new FormGroup({
      name: new FormControl(null),
    });
  }
}

describe('TrimDirective', () => {
  let fixture: ComponentFixture<UnitTestComponent>;
  let des: DebugElement[]; // elements w/ the directive

  beforeEach(() => {
    fixture = TestBed.configureTestingModule({}).createComponent(
      UnitTestComponent,
    );

    fixture.detectChanges(); // initial binding

    // all elements with an attached TrimDirective
    des = fixture.debugElement.queryAll(By.directive(TrimDirective));
  });

  it('should have 2 inputs with trim directive', () => {
    expect(des.length).toBe(2);
  });

  it('should trim leading and trailing spaces', () => {
    const nativeElement = des[0].nativeElement;
    nativeElement.value = ' test ';
    expect(nativeElement.value).toBe(' test ');

    const inputEvent = new InputEvent('blur');
    nativeElement.dispatchEvent(inputEvent);

    expect(nativeElement.value).toBe('test');
  });

  it('should return an empty string if the value consists of spaces', () => {
    const nativeElement = des[0].nativeElement;
    nativeElement.value = '   ';
    expect(nativeElement.value).toBe('   ');

    const inputEvent = new InputEvent('blur');
    nativeElement.dispatchEvent(inputEvent);

    expect(nativeElement.value).toBe('');
  });

  it('should trim and update ngControl on blur', () => {
    const formControl = fixture.componentInstance.form.controls.name;
    formControl.setValue(' test ');
    expect(formControl.value).toBe(' test ');

    const nativeElement = des[1].nativeElement;
    expect(nativeElement.value).toBe(' test ');

    const inputEvent = new InputEvent('blur');
    nativeElement.dispatchEvent(inputEvent);

    fixture.detectChanges();
    fixture.whenStable().then(() => {
      expect(formControl.value).toBe('test');
    });
  });

  it('should set ngControl value to null if the value is an empty string', () => {
    const nativeElement = des[1].nativeElement;
    nativeElement.value = '';
    expect(nativeElement.value).toBe('');

    const inputEvent = new InputEvent('blur');
    nativeElement.dispatchEvent(inputEvent);

    fixture.detectChanges();
    fixture.whenStable().then(() => {
      const formControl = fixture.componentInstance.form.controls.name;
      expect(formControl.value).toBe(null);
    });
  });
});
