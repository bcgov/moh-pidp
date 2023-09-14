import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SafePipe } from '../../../../pipes';
import { HtmlComponent } from './html.component';

describe('HtmlComponent', () => {
  let component: HtmlComponent;
  let fixture: ComponentFixture<HtmlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HtmlComponent, SafePipe],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HtmlComponent);
    component = fixture.componentInstance;
    component.data = { content: '' };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
