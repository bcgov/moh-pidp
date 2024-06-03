import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkAccountConfirmPage } from './link-account-confirm.page';

describe('LinkAccountConfirmPage', () => {
  let component: LinkAccountConfirmPage;
  let fixture: ComponentFixture<LinkAccountConfirmPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LinkAccountConfirmPage],
    }).compileComponents();

    fixture = TestBed.createComponent(LinkAccountConfirmPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
