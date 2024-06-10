import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { NeedHelpComponent } from './need-help.component';

describe('NeedHelpComponent', () => {
  let component: NeedHelpComponent;
  let fixture: ComponentFixture<NeedHelpComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NoopAnimationsModule],
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(NeedHelpComponent);
    component = fixture.componentInstance;

    fixture.detectChanges();
  });

  describe('Default state', () => {
    given('the component as default values', () => {
      when('the component has been initialized', () => {
        fixture.detectChanges();

        then(
          'the help section should contain "Need help?" text and help panel hidden',
          () => {
            const needHelpLink = fixture.debugElement.query(
              By.css('.need-help-header button.pidp-btn-link'),
            );
            expect(needHelpLink.nativeElement.innerHTML).toContain(
              'Need Help?',
            );

            const expansionPanelContent = fixture.debugElement.query(
              By.css('.expansion-panel-content'),
            );
            expect(expansionPanelContent.styles.visibility).toBe('hidden');
          },
        );
      });
    });
  });

  describe('METHOD: onShowNeedHelp', () => {
    given('the panel is hidden', () => {
      component.showNeedHelp = false;

      when('the onShowNeedHelp method is invoked', () => {
        component.onShowNeedHelp();
        fixture.detectChanges();

        then('the help panel should be visible', () => {
          expect(component.showNeedHelp).toBeTruthy();
          const expansionPanelContent = fixture.debugElement.query(
            By.css('.expansion-panel-content'),
          );
          expect(expansionPanelContent.styles.visibility).toBe('');
        });
      });
    });

    given('the panel is shown', () => {
      component.showNeedHelp = true;

      when('the onShowNeedHelp method is invoked', () => {
        component.onShowNeedHelp();
        fixture.detectChanges();

        then('the help panel should be hidden', () => {
          expect(component.showNeedHelp).toBeFalsy();
          const expansionPanelContent = fixture.debugElement.query(
            By.css('.expansion-panel-content'),
          );
          expect(expansionPanelContent.styles.visibility).toBe('hidden');
        });
      });
    });
  });

  describe('show or hide the icon', () => {
    given('the icon is hidden', () => {
      component.showIcon = false;

      when('the component has been initialized', () => {
        fixture.detectChanges();

        then('the icon should be hidden', () => {
          expect(component.showIcon).toBeFalsy();
          const icon = fixture.debugElement.query(
            By.css('.need-help-header button i'),
          );
          expect(icon).toBeNull();
        });
      });
    });

    given('the icon is shown', () => {
      component.showIcon = true;

      when('the component has been initialized', () => {
        fixture.detectChanges();

        then('the icon should be shown', () => {
          expect(component.showIcon).toBeTruthy();
          const icon = fixture.debugElement.query(
            By.css('.need-help-header button i'),
          );
          expect(icon).toBeDefined();
        });
      });
    });
  });

  describe('PROPERTY: set customClass', () => {
    given('there is no custom class', () => {
      when('the component has been initialized', () => {
        fixture.detectChanges();

        then('should return 1 class', () => {
          const classes = component.customClasses;
          expect(classes.length).toBe(1);
          expect(classes[0]).toBe('need-help');
        });
      });
    });

    given('there is "need-help-large" custom class', () => {
      component.customClass = 'need-help-large';

      when('the component has been initialized', () => {
        fixture.detectChanges();

        then('should return 2 classes', () => {
          const classes = component.customClasses;
          expect(classes.length).toBe(2);
          expect(classes[0]).toBe('need-help');
          expect(classes[1]).toBe('need-help-large');
        });
      });
    });
  });
});
