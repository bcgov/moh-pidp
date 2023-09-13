import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatMenuModule } from '@angular/material/menu';

import { ContextHelpComponent } from './context-help.component';

describe('ContextHelpComponent', () => {
  let component: ContextHelpComponent;
  let fixture: ComponentFixture<ContextHelpComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ContextHelpComponent],
      imports: [MatMenuModule],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContextHelpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
