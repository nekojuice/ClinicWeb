import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Fappointment2Component } from './fappointment2.component';

describe('Fappointment2Component', () => {
  let component: Fappointment2Component;
  let fixture: ComponentFixture<Fappointment2Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [Fappointment2Component]
    });
    fixture = TestBed.createComponent(Fappointment2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
