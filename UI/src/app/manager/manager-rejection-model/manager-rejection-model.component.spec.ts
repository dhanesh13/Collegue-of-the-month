import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerRejectionModelComponent } from './manager-rejection-model.component';

describe('ManagerRejectionModelComponent', () => {
  let component: ManagerRejectionModelComponent;
  let fixture: ComponentFixture<ManagerRejectionModelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManagerRejectionModelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagerRejectionModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
