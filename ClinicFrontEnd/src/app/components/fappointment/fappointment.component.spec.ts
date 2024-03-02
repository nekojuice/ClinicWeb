import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FAppointmentComponent } from './fappointment.component';

describe('FAppointmentComponent', () => {
  let component: FAppointmentComponent;
  let fixture: ComponentFixture<FAppointmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FAppointmentComponent]
    });
    fixture = TestBed.createComponent(FAppointmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
