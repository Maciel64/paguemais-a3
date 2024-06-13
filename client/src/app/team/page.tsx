import Image from "next/image";

const Team = () => {
  return (
    <div className="flex items-center flex-col gap-5">
      <Image src="/file.jpg" alt="Logo" width={200} height={200} />
      <Image src="/equipe.jpeg" alt="Logo" width={600} height={400} />
      <ul className="flex gap-8">
        <li>Maciel Suassuna</li>
        <li>Felipe Tauan</li>
        <li>Artur Furtado</li>
        <li>João Victor</li>
      </ul>
    </div>
  );
};

export default Team;
