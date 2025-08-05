import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignarMedicoComponent } from './asignar-medico.component';

describe('AsignarMedicoComponent', () => {
  let component: AsignarMedicoComponent;
  let fixture: ComponentFixture<AsignarMedicoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AsignarMedicoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AsignarMedicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
