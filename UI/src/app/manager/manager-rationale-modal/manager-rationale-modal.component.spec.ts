import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerRationaleModalComponent } from './manager-rationale-modal.component';

describe('ManagerRationaleModalComponent', () => {
  let component: ManagerRationaleModalComponent;
  let fixture: ComponentFixture<ManagerRationaleModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManagerRationaleModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagerRationaleModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
