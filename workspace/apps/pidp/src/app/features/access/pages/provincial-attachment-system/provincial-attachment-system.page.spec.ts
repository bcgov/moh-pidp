import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvincialAttachmentSystemPage } from './provincial-attachment-system.page';

describe('ProvincialAttachmentSystemPage', () => {
  let component: ProvincialAttachmentSystemPage;
  let fixture: ComponentFixture<ProvincialAttachmentSystemPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProvincialAttachmentSystemPage]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProvincialAttachmentSystemPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
