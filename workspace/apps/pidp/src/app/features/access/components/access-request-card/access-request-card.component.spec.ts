import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessRequestCardComponent } from './access-request-card.component';

describe('AccessRequestCardComponent', () => {
  let component: AccessRequestCardComponent;
  let fixture: ComponentFixture<AccessRequestCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccessRequestCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AccessRequestCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
