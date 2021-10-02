import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CloseSessionConfirmationComponent } from './close-session-confirmation.component';

describe('CloseSessionConfirmationComponent', () => {
  let component: CloseSessionConfirmationComponent;
  let fixture: ComponentFixture<CloseSessionConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CloseSessionConfirmationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CloseSessionConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
