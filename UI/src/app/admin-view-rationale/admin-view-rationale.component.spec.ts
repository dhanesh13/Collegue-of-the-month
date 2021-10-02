import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminViewRationaleComponent } from './admin-view-rationale.component';

describe('AdminViewRationaleComponent', () => {
  let component: AdminViewRationaleComponent;
  let fixture: ComponentFixture<AdminViewRationaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminViewRationaleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminViewRationaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
