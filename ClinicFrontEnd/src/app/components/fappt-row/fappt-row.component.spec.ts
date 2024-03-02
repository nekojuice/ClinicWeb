import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FapptRowComponent } from './fappt-row.component';

describe('FapptRowComponent', () => {
  let component: FapptRowComponent;
  let fixture: ComponentFixture<FapptRowComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FapptRowComponent]
    });
    fixture = TestBed.createComponent(FapptRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
