import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminBasketModalComponent } from './admin-basket-modal.component';

describe('AdminBasketModalComponent', () => {
  let component: AdminBasketModalComponent;
  let fixture: ComponentFixture<AdminBasketModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminBasketModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminBasketModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
