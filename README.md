# ComplexIT_DatabaseSuff

(assuming you have psql)
Creating database:

psql -U postgres -c "create database complexit_files"

---

Creating the table:

create table files (
  id SERIAL primary key,
  name VARCHAR not null,
  room VARCHAR not null,
  data bytea not null,
  upload_date TIMESTAMP not null default NOW()
);


Change the conn string, so it fits yours and the file stream path aswell, but they may not be required if you feed it a formdata format directly
