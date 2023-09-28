import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SharedUiModule } from '@bcgov/shared/ui';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { NeedHelpComponent } from './need-help.component';

describe('NeedHelpComponent', () => {
  let fixture: ComponentFixture<NeedHelpComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NeedHelpComponent],
      imports: [NoopAnimationsModule, SharedUiModule],
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(NeedHelpComponent);
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
              By.css('.need-help-header a')
            );
            expect(needHelpLink.nativeElement.innerHTML).toContain(
              'Need Help?'
            );

            const expansionPanelContent = fixture.debugElement.query(
              By.css('.expansion-panel-content')
            );
            expect(expansionPanelContent.styles.visibility).toBe('hidden');
          }
        );
      });
    });
  });
});
