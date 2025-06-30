using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
//Bağlantı oluşturma
var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};
//Bağlantı aktifleştirme ve kanal oluşturmaa
using IConnection connection=factory.CreateConnection();
using IModel channel=connection.CreateModel();

//Queue oluşturma
channel.QueueDeclare(queue: "example", exclusive: false,durable:true);

//Queueden mesaj okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example",false,consumer);
channel.BasicQos(0, 1, false);
consumer.Received += (sender, e) =>
{
    //Kuyruğa gelen mesajın işlendiği yer.
    //e.body kuyruktaki mesajın verisini getirecektir.
    //e.body.span veya e.body.toarray() kuyruktaki mesajın byte verisini çeker.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    channel.BasicAck(deliveryTag: e.DeliveryTag,multiple:false);
};
Console.ReadLine();