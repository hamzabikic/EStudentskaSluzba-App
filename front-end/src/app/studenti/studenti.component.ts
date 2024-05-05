import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../MojConfig";

@Component({
  selector: 'app-studenti',
  templateUrl: './studenti.component.html',
  styleUrls: ['./studenti.component.css']
})
export class StudentiComponent implements OnInit {

  studenti:any;
  studenti_copy:any;
  imeprezime ="";
  otvori = false;
  ime ="";
  prezime ="";
  datumRodjenja = new Date();
  opstinaId =0;
  email ="";
  korisnickoIme="";
  brojIndeksa="";
  smjerId =0;
  godinaStudija =1;
  smjerovi:any;
  opstine:any;
  dialog_novi_student = false;
  moguce_slanje = true;

  constructor(private http : HttpClient) { }

  async ngOnInit() {
    await this.ucitajStudente();
    await this.ucitajSmjerove();
    await this.ucitajOpstine();
  }
  async dodajStudenta() {
     let obj = {
       ime: this.ime,
       prezime:this.prezime,
       datumRodjenja: this.datumRodjenja,
       opstinaId:this.opstinaId,
       korisnickoIme:this.korisnickoIme,
       email:this.email,
       brojIndeksa:this.brojIndeksa,
       godinaStudija :this.godinaStudija,
       smjerId:this.smjerId
     };
     this.moguce_slanje = false;
     let res = await this.http.post(MojConfig.adresa_servera+"/Student/addStudent",obj).toPromise();
     // @ts-ignore
    if(res.dodan){
       // @ts-ignore
      alert("Uspjesno dodan student sa id: " +  res.id);
      this.moguce_slanje=true;
       await this.ucitajStudente();
       this.pretraga();
       this.obrisiPodatke();
       return;
     }
    // @ts-ignore
    alert(res.greska);
    this.moguce_slanje= true;
  }
  async ucitajSmjerove() {
    this.smjerovi = await this.http.get(MojConfig.adresa_servera+"/Student/getSmjerovi").toPromise();
  }
  async ucitajOpstine() {
    this.opstine = await this.http.get(MojConfig.adresa_servera +"/Opstina/getOpstine").toPromise();
  }
  async ucitajStudente() {
     this.studenti = await this.http.get(MojConfig.adresa_servera + "/Student/getStudenti")
       .toPromise();
     this.studenti_copy = this.studenti;
  }
  async obrisiStudenta(id:number) {
    let res = await this.http.delete(MojConfig.adresa_servera+"/Student/deleteStudent?id="+ id).toPromise();
    if(res) {
      alert("Uspjesno obrisan student!");
      await this.ucitajStudente();
      this.pretraga();
      return;
    }
    alert("Neuspjesno brisanje studenta!");
  }
  pretraga() {
     this.studenti_copy = this.studenti.filter((s:any)=> (s.ime + " " + s.prezime).toLowerCase()
       .includes(this.imeprezime.toLowerCase()));
  }

  protected readonly Date = Date;

  obrisiPodatke() {
    this.ime ="";
    this.prezime ="";
    this.brojIndeksa ="";
    this.smjerId=0;
    this.opstinaId =0;
    this.email ="";
    this.korisnickoIme ="";
    this.datumRodjenja = new Date();
    this.godinaStudija =1;
  }
}
