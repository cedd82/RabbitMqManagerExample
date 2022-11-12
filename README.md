# basicExampleWorkingWithRabbitMq

uses curl cmds to download messages off queues to a local folder and push messages to a queue from a local folder

example use case:

An error queue has 100 messages due to a bug in the code. 
- configure the app to connect to the queue, the code will download all messages, deseralise to the type (optional) 
and write each message as a json file in a folder c:/temp{datetime.now.ticks}
- run the app in a mode to push msgs onto the process queue with the fix from the msgs downloaded in the step above. 
