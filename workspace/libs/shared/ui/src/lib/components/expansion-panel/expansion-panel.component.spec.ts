import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { ExpansionPanelComponent } from './expansion-panel.component';

describe('ExpansionPanelComponent', () => {
  let component: ExpansionPanelComponent;
  let fixture: ComponentFixture<ExpansionPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NoopAnimationsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(ExpansionPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  describe('Default state', () => {
    given('the component as default values', () => {
      when('the component has been initialized', () => {
        then('the panel is collapsed', () => {
          const expansionPanelContent = fixture.debugElement.query(
            By.css('.expansion-panel-content'),
          );
          expect(expansionPanelContent.styles.visibility).toBe('hidden');
          expect(component.expanded).toBeFalsy;
        });
      });
    });
  });

  describe('Panel should be expanded', () => {
    given('expanded property is true', () => {
      component.expanded = true;

      when('the component has been initialized', () => {
        fixture.detectChanges();

        then('the panel is expanded', () => {
          const expansionPanelContent = fixture.debugElement.query(
            By.css('.expansion-panel-content'),
          );
          expect(expansionPanelContent.styles.visibility).toBe('');
          expect(component.expanded).toBeTruthy;
        });
      });
    });
  });

  describe('METHOD: toggle()', () => {
    given('user wants to expand the panel', () => {
      when('toggle is invoked', () => {
        component.toggle();
        fixture.detectChanges();

        then('the panel is collapsed', () => {
          const expansionPanelContent = fixture.debugElement.query(
            By.css('.expansion-panel-content'),
          );
          expect(expansionPanelContent.styles.visibility).toBe('');
          expect(component.expanded).toBeTruthy;
        });
      });
    });

    given('user wants to collapse the panel', () => {
      component.expanded = true;
      when('toggle is invoked', () => {
        component.toggle();
        fixture.detectChanges();

        then('the panel is collapsed', () => {
          const expansionPanelContent = fixture.debugElement.query(
            By.css('.expansion-panel-content'),
          );
          expect(expansionPanelContent.styles.visibility).toBe('hidden');
          expect(component.expanded).toBeFalsy;
        });
      });
    });
  });
});
